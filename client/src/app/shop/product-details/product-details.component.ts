import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product:IProduct | any;
  constructor(private shopService:ShopService, private activateRoute:ActivatedRoute) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    this.shopService.getProduct(+this.activateRoute.snapshot.paramMap.get('id')!).subscribe(product=>{
      this.product=product;
    },error=>{
      console.log(error);
    })
  }
}
