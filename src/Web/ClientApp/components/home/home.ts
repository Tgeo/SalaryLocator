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
    private currentAreas: any[] = [];

    private selectedAreaCode : number;
    private selectedSalary : Salary;

    constructor(private salaryService : SalaryService) {
    }

    ngOnInit() {
        this.getOccupations();
    }

    getOccupations() {
        this.salaryService.getOccupations()
            .then(occupations => this.occupations = occupations);
    }

    // getStates() {
    //     this.salaryService.getStates()
    //         .then(states => this.states = states);
    // }

    occupationSelected(occupationCode : string) {
        if (!occupationCode) return;
        this.salaryService.getAreasWithHighestSalaries(occupationCode)
            .then(areas => this.currentAreas = areas);
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
}
