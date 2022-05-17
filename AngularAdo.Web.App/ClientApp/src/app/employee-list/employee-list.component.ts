import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Employee } from '../Employee';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css'],
})
export class EmployeeListComponent implements OnInit {
  employes?: Employee[];
  current?: Employee;
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadEmployes();
  }

  loadEmployes(): void {
    this.http.get<Employee[]>('api/v1/employee/all').subscribe(
      (response) => {
        this.employes = response;
      },
      (error) => console.log(error)
    );
  }

  onSelected(employee: Employee): void {
    this.current = employee;
  }

  onNewEmployee(): void {
    this.current = new Employee(0, '', '', '', '', 0, '');
  }
}
