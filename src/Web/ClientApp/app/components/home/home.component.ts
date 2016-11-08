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
        trigger('flyInOut', [
            state('in', style({transform: 'translateX(0)'})),
            transition('void => *', [
                style({transform: 'translateX(-100%)'}),
                animate(500)
            ]),
            transition('* => void', [
                animate(500, style({transform: 'translateX(100%)'}))
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
        },
        title: {
            display: true,
            text: 'Highest Paying Cities by Occupation'
        }
    };
    public barChartLabels: any[] = [];
    public barChartData: any[] = [
        { data: [], label: 'Annual' },
        { data: [], label: 'Hourly' }
    ];

    // Whether this is being rendered on browser or server.
    private isBrowser : boolean = universal.isBrowser;

    private areas: Area[];
    private occupations: Occupation[];
    private selectedAreaCode : number;

    constructor(private readonly salaryService : SalaryService) {
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
        this.getAreasWithHighestSalaries(occupationCode);
        this.salaryService.getAreasWithOccupation(occupationCode)
            .then(areas => this.areas = areas);
    }

    getAreasWithHighestSalaries(occupationCode : string) {
        if (!occupationCode) return;
        this.salaryService.getAreasWithHighestSalaries(occupationCode)
            .then(areas => this.initChart(areas));
    }

    areaSelected(areaCode : number) {
        this.selectedAreaCode = areaCode;
        if (!this.selectedAreaCode) return;
        this.salaryService.getOccupations()
            .then(occupations => this.occupations = occupations);
    }

    selectFirstOccupation() {
        if (!this.occupations || this.occupations.length <= 0) return;
        this.getAreasWithHighestSalaries(this.occupations[0].code);
    }

    initChart(areasWithSalary : any[]) {
        if (!areasWithSalary || !this.isBrowser) return;

        this.barChartLabels.length = 0; 
        let labels = areasWithSalary.map(a => this.formatCityLabel(a)).reverse();
        // Have to add to the original array or else Angular won't see the change. Annoying.
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
