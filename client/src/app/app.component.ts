import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { Pagination } from './layout/Models/pagination';
import { Product } from './layout/Models/products';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})

export class AppComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/'
  title = 'client';
  http = inject(HttpClient);
  products: Product[] = [];

  ngOnInit(): void{
    this.http.get<Pagination<Product>>(this.baseUrl + 'products/2').subscribe({
      next: response => this.products = response.data,
      error: error => console.log(error),
      complete: () => console.log('complete')
    })
  }
}
