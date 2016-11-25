import { Injectable }    from '@angular/core';
import { Http, Response } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { State } from '../models/state';
import { Area } from '../models/area';
import { Occupation } from '../models/occupation';
import { Salary } from '../models/salary';

@Injectable()
export class SalaryService {

    constructor(private http: Http) { }

    getOccupations() : Promise<Occupation[]> {
        let url = 'http://localhost:52463/api/occupation';
        return this.http.get(url)
            .toPromise()
            .then(response => response.json() as Occupation[])
            .catch(this.handleError);
    }

    getAreasWithOccupation(occupationCode : string) : Promise<Area[]> {
        return this.http.get('http://localhost:52463/api/location/area/' + occupationCode)
            .toPromise()
            .then(response => response.json() as Area[])
            .catch(this.handleError);
    }

    getAreasWithHighestSalaries(occupationCode : string) : Promise<any> {
        let url = 'http://localhost:52463/api/location/area.HighestSalaries?occupationCode=' + occupationCode;
        return this.http.get(url)
            .toPromise()
            .then(response => response.json())
            .catch(this.handleError);
    }

    getAreasWithHighestSalariesAdjustedForCol(occupationCode : string) : Promise<any> {
        let url = 'http://localhost:52463/api/location/area.HighestSalaries?occupationCode=' + occupationCode
            + '&adjustForCostOfLiving=true';
        return this.http.get(url)
            .toPromise()
            .then(response => response.json())
            .catch(this.handleError);
    }

    // Gets a salary for a specific area and occupation.
    getSalary(areaCode : number, occupationCode : string) : Promise<Salary> {
        let url = 'http://localhost:52463/api/salary?areaCode=' + areaCode + '&occupationCode=' + occupationCode;
        return this.http.get(url)
            .toPromise()
            .then(response => response.json() as Salary)
            .catch(this.handleError);
    }

    private handleError(error: any) {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}
