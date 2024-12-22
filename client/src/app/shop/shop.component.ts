import { HttpClient } from '@angular/common/http';
import { ShopService } from './shop.service';
import { Component, OnInit } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/type';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  products: IProduct[];
  types: IType[];
  sortOptions = [
    { name: 'Price: Low to High', value: 'priceasc' },
    { name: 'Price: High to Low', value: 'pricedesc' },
    { name: 'Name', value: 'name' },
  ];
  selectedTypeId: number = 0;
  baseUrl = 'https://localhost:5001/api/products';
  constructor(private shopService: ShopService, private http: HttpClient) {}

  ngOnInit() {
    this.getProducts();
    this.getTypes();
  }
  getProducts(typeId?: number, sort?: string) {
    this.shopService.getProducts(typeId, sort).subscribe(
      (response) => {
        this.products = response;
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getTypes() {
    this.shopService.getTypes().subscribe(
      (response) => {
        this.types = response;
      },
      (error) => {
        console.log(error);
      }
    );
  }
    // Handle type selection
    onTypeSelected(typeId: number) {
      this.selectedTypeId = typeId;
      this.getProducts(typeId, undefined); // Fetch products based on the selected typeId
    }
  onSortSelected(sort: string) {
    this.getProducts(this.selectedTypeId, sort);
  }

}
