import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Skinet';

  constructor(private httpClient:HttpClient){}
  ngOnInit(): void {
    this.httpClient.get("https://localhost:7265/api/Products").subscribe((response:any)=>{
      console.log(response);
    },error=>{
      console.log(error);
    })
  }
}
