import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface IWeather {
  id: string;
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent implements OnInit {
  public weathers: IWeather[];
  constructor(private http: HttpClient) {
  }

  ngOnInit() {
    this.http.get(`http://localhost:5000/weatherforecast`)
    .subscribe((data: IWeather[]) => { this.weathers = data;  console.log(this.weathers); }, (error) => { console.log(error); });
  }

}
