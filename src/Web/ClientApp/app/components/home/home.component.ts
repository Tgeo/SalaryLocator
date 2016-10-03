import { Component, OnInit } from '@angular/core';
import * as universal from 'angular2-universal';
import { CurrencyPipe } from '@angular/common';
import { SalaryService } from '../services/salary.service';
import { State } from '../models/state';
import { Area } from '../models/area';
import { Occupation } from '../models/occupation';
import { Salary } from '../models/salary';

@Component({
    selector: 'home',
    template: require('./home.component.html'),
    providers: [ SalaryService ]
})
export class HomeComponent implements OnInit {
  
    // Whether this is being rendered on browser or server.
    private isBrowser : boolean = universal.isBrowser;

    public barChartOptions: any = {
        scaleShowVerticalLines: false,
        scales: {
            yAxes: [{
                ticks: { beginAtZero: true, },
                position: "left",
                id: "y-axis-1",
            }, {
                ticks: { beginAtZero: true },
                position: "right",
                id: "y-axis-2",
            }]
        },
        title: {
            display: true,
            text: 'Highest Paying Cities by Occupation'
        }
    };
    public barChartLabels: string[] = [];
    public barChartData: any[] = [
        { data: [], label: 'Annual' },
        { data: [], label: 'Hourly' }
    ];

    private states: State[] = [];
    private areas: Area[] = [];
    private occupations: Occupation[] = [];

    private selectedAreaCode : number;
    private selectedSalary : Salary;

    constructor(private salaryService : SalaryService) {
    }

    ngOnInit() {
        this.getOccupations();
    }

    getOccupations() {
        this.salaryService.getOccupations()
            .then(occupations => {
                this.occupations = occupations;
                this.selectFirstOccupation();
            });
    }

    occupationSelected(occupationCode : string) {
        if (!occupationCode) return;
        this.salaryService.getAreasWithHighestSalaries(occupationCode)
            .then(areas => this.initChart(areas));
    }

    initChart(areasWithSalary : any[]) {
        if (!areasWithSalary || !this.isBrowser) return;

        this.barChartLabels.length = 0; 
        let labels = areasWithSalary.map(a => a.name + ', ' + a.primaryStateCode).reverse();
        // Have to add to the original array or else Angular won't see the change.
        while (labels.length > 0) {
            this.barChartLabels.push(labels.pop());
        }

        let annualSalaryData = areasWithSalary.map(a => a.annualMedianPercentile);
        let hourlySalaryData = areasWithSalary.map(a => a.hourlyMedianPercentile);
        this.barChartData = [
            { data: annualSalaryData, label: 'Annual', yAxisID: 'y-axis-1' },
            { data: hourlySalaryData, label: 'Hourly', yAxisID: 'y-axis-2' }
        ];
    }

    stateSelected(stateCode : string) {
        if (!stateCode) return;
        this.salaryService.getAreas(stateCode)
            .then(areas => this.areas = areas);
    }

    areaSelected(areaCode : number) {
        this.selectedAreaCode = areaCode;
        if (!this.selectedAreaCode) return;
        this.salaryService.getOccupations()
            .then(occupations => this.occupations = occupations);
    }

    selectFirstOccupation() {
        if (!this.occupations || this.occupations.length <= 0) return;
        this.occupationSelected(this.occupations[0].code);
    }
}
