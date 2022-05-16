import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit  {
  public forecasts: WeatherForecast[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    //http.get<WeatherForecast[]>(baseUrl + 'api/weatherforecast').subscribe(result => {
    //  this.forecasts = result;
    //}, error => console.error(error));
  }
  ngOnInit(): void {    
    var body = {
      "userName": "string",
      "password": "string"
    };

    
    
    this.http.post(this.baseUrl + "api/v1/Authentication/LoginUser", body, { responseType: 'text' })
      .subscribe((token: string) =>
      {
        debugger
        const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + token
          })
        };

        this.http.get<WeatherForecast[]>(this.baseUrl + 'api/weatherforecast', httpOptions).subscribe(result => {
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
