import { Component, OnInit } from '@angular/core';
import { SalaryService } from '../services/salary.service';
import { State } from '../models/state';
import { Area } from '../models/area';
import { Occupation } from '../models/occupation';
import { Salary } from '../models/salary';

@Component({
    selector: 'home',
    template: require('./home.html'),
    providers: [ SalaryService ]
})
export class Home implements OnInit {
    
    private states: State[] = [];
    private areas: Area[] = [];
    private occupations: Occupation[] = [];
    
    private selectedAreaCode : number;
    private selectedSalary : Salary;

    constructor(private salaryService : SalaryService) {
    }

    ngOnInit() {
        this.getStates();
    }

    getStates() {
        this.salaryService.getStates()
            .then(states => this.states = states);
    }

    stateSelected(stateCode : string) {
        if (!stateCode) return;
        this.salaryService.getAreas(stateCode)
            .then(areas => this.areas = areas);
    }

    areaSelected(areaCode : number) {
        this.selectedAreaCode = areaCode;
        if (!this.selectedAreaCode) return;
        this.salaryService.getOccupations(this.selectedAreaCode)
            .then(occupations => this.occupations = occupations);
    }

    occupationSelected(occupationCode : string) {
        this.salaryService.getSalaryData(this.selectedAreaCode, occupationCode)
            .then(salary => this.selectedSalary = salary);
    }
}
