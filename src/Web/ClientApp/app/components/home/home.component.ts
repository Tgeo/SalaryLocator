import { Component, OnInit, trigger, state, transition, style, animate } from '@angular/core';
import * as universal from 'angular2-universal';
import { CurrencyPipe } from '@angular/common';
import { SalaryService } from '../services/salary.service';
import { State } from '../models/state';
import { Area } from '../models/area';
import { Occupation } from '../models/occupation';
import { Salary } from '../models/salary';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    providers: [ SalaryService ],
    animations: [
        trigger('flyIn', [
            state('in', style({transform: 'translateX(0)'})),
            transition('void => *', [
                style({transform: 'translateX(-100%)'}),
                animate(500)
            ])
        ])
    ]
})
export class HomeComponent implements OnInit {

    private readonly maxLabelChars = 15;

    // Chart.js options:
    public barChartOptions: any = {
        scales: {
            xAxes: [ { gridLines : { display : false } } ],
            yAxes: [{
                ticks: { beginAtZero: true },
                position: "left",
                id: "y-axis-1",
                gridLines: { display : false }
            }, {
                ticks: { beginAtZero: true },
                position: "right",
                id: "y-axis-2",
                gridLines: { display : false }
            }]
        }
    };
    public barChartLabels: any[] = [];
    public barChartData: any[] = [
        { data: [], label: 'Annual' },
        { data: [], label: 'Hourly' }
    ];

    // Whether this is being rendered on browser or server.
    // This is needed because rendering Charts server-side causes issues.
    // Thus, we can choose not to render things until we are on the client.
    private isBrowser : boolean = universal.isBrowser;

    private areas: Area[];
    private occupations: Occupation[];
    private selectedOccupationCode : string;
    private selectedAreaCode : number;
    private selectedSalaryAmount : number;
    private selectedSalary : Salary;

    constructor(private readonly salaryService : SalaryService) {
    }

    ngOnInit() {
        this.loadOccupations();
    }

    loadOccupations() {
        this.salaryService.getOccupations()
            .then(occupations => {                
                occupations.unshift(new Occupation());
                this.occupations = occupations;
            });
    }

    occupationSelected(occupationCode : string) {
        this.selectedOccupationCode = occupationCode;
        if (!this.selectedOccupationCode) return;

        // Get areas where the occupation's salary is highest.
        this.getAreasWithHighestSalaries(this.selectedOccupationCode);
        // Get a list of areas where the occupation is present. 
        this.salaryService.getAreasWithOccupation(this.selectedOccupationCode)
            .then(areas => {
                areas.unshift(new Area());
                this.areas = areas;
            });
    }

    salaryChanged(salary : number) {
        if (!salary || salary <= 0)
            this.selectedSalaryAmount = null;
        else
            this.selectedSalaryAmount = salary;
    }

    hourlyChanged(hourly : number) {
        if (!hourly || hourly <= 0)
            this.selectedSalaryAmount = null;
        else
            this.selectedSalaryAmount = Math.round(hourly * 40 * 52);
    }

    getAreasWithHighestSalaries(occupationCode : string) {
        if (!occupationCode) return;
        this.salaryService.getAreasWithHighestSalaries(occupationCode)
            .then(areas => this.initChart(areas));
    }

    areaSelected(areaCode : number) {
        this.selectedAreaCode = areaCode;
        if (!this.selectedAreaCode) return;

        this.salaryService.getSalary(this.selectedAreaCode, this.selectedOccupationCode)
            .then(salary => this.selectedSalary = salary);

        this.salaryService.getAreasWithHighestSalariesAdjustedForCol(this.selectedOccupationCode)
            .then(areas => this.initChart(areas));
    }

    initChart(areasWithSalary : any[]) {
        if (!areasWithSalary || !this.isBrowser) return;

        this.barChartLabels.length = 0; 
        let labels = areasWithSalary.map(a => this.formatCityLabel(a)).reverse();
        // Have to add to the original array or else Angular won't see the change. Annoying.
        while (labels.length > 0) {
            this.barChartLabels.push(labels.pop());
        }
        console.log(JSON.stringify(this.barChartLabels));

        let annualSalaryData = areasWithSalary.map(a => a.annualMedianPercentile);
        let hourlySalaryData = areasWithSalary.map(a => a.hourlyMedianPercentile);
        this.barChartData = [
            { data: annualSalaryData, label: 'Annual', yAxisID: 'y-axis-1' },
            { data: hourlySalaryData, label: 'Hourly', yAxisID: 'y-axis-2' }
        ];
console.log(JSON.stringify(this.barChartData));

    }

    formatCityLabel(areaWithSalary) : any {
        if (!areaWithSalary) return;

        if (areaWithSalary.name.length > this.maxLabelChars) {
            // Chart.js will take an array of strings for a label.
            let name : string = areaWithSalary.name;
            let nameArr = name.split('-').filter(t => t !== '');
            nameArr[nameArr.length - 1] = nameArr[nameArr.length - 1] + ', ' + areaWithSalary.primaryStateCode;
            return nameArr;
        }

        return areaWithSalary.name + ', ' + areaWithSalary.primaryStateCode;
    }
}
