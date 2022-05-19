import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';

import * as moment from 'moment';
import { Employee } from '../Employee';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.css'],
})
export class EmployeeDetailComponent implements OnInit {
  @Input() model?: Employee;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  save(): void {
    var body: Employee = Object.assign({}, this.model);
    body.fechaNacimiento = moment(this.model?.fechaNacimiento).toISOString();
    if (body.id === 0) {
      this.createEmployee(body);
    } else {
      this.updateEmployee(body);
    }
  }

  onDateChange(employee: Employee) {
    debugger;
    return moment(employee.fechaNacimiento).format('yyyy-MM-DD');
  }

  private getEmployee(id: number) {
    this.http.get<Employee>(`api/v1/employee/${id}`).subscribe(
      (employee) => {
        this.model = employee;
        this.model.fechaNacimiento = moment(this.model.fechaNacimiento).format(
          'yyyy-MM-DD'
        );
      },
      (error) => console.log(error)
    );
  }

  private updateEmployee(body: Employee) {
    if (body !== null) {
      this.http
        .put(`api/v1/employee/${body.id}`, body, { responseType: 'text' })
        .subscribe(
          (response) => {
            this.getEmployee(body.id);
            console.log(response);
          },
          (error) => console.error(error)
        );
    }
  }

  private createEmployee(body: Employee) {
    if (body !== null) {
      this.http
        .post('api/v1/employee', body, { responseType: 'text' })
        .subscribe(
          (response) => {
            console.log(response);
          },
          (error) => console.error(error)
        );
    }
  }
}
