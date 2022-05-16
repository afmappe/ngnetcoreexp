import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];

  constructor(private http: HttpClient, private authService: AuthService) {
  }
  ngOnInit(): void {
    this.authService.login("email", "password").subscribe((token: boolean) => {
      this.http.get<WeatherForecast[]>('api/weatherforecast').subscribe(result => {
        this.forecasts = result;
      });
    });
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
