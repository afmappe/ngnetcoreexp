import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import * as moment from 'moment';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.css'],
})
export class EmployeeDetailComponent implements OnInit {
  public model?: Employee;

  constructor(
    private http: HttpClient,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initComponent();
  }

  initComponent(): void {
    const id = parseInt(this.activatedRoute.snapshot.paramMap.get('id')!, 10);
    if (id !== 0) {
      this.getEmployee(id);
    } else {
      this.model = new Employee(0, '', '', '', '', 0, '');
    }
  }

  save(): void {
    var body: Employee = Object.assign({}, this.model);
    body.fechaNacimiento = moment(this.model?.fechaNacimiento).toISOString();
    if (body.id === 0) {
      this.createEmployee(body);
    } else {
      this.updateEmployee(body);
    }
  }

  newEmployee(): void {
    this.model = new Employee(0, '', '', '', '', 0, '');
    this.router.navigateByUrl('/employee-detail/0');
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

export class Employee {
  constructor(
    public id: number,
    public apellidos: string,
    public direccion: string,
    public email: string,
    public nombres: string,
    public sueldo: number,
    public telefono: string,
    public fechaNacimiento?: string
  ) {}
}
