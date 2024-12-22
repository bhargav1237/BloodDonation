import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/type';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  // getProducts() {
  //   return this.http.get<IProduct[]>(this.baseUrl + 'products');
  // }

  // Get products with optional sorting
  // getProducts(sort?: string) {
  //   let url = this.baseUrl + 'products';
  //   if (sort) {
  //     url += `?sort=${sort}`;
  //   }
  //   return this.http.get<IProduct[]>(url);
  // }

  getProducts(typeId?: number, sort?: string): Observable<IProduct[]> {
    let url = this.baseUrl + 'products';

    // If a typeId is provided, add it as a query parameter
    if (typeId) {
      url += `?typeId=${typeId}`;
    }

    // If a sort is provided, add it as another query parameter
    if (sort) {
      url += (url.includes('?') ? '&' : '?') + `sort=${sort}`;
    }

    return this.http.get<IProduct[]>(url);
  }
  getTypes() {
    let url = this.baseUrl + 'products';
    return this.http.get<IType[]>(url + '/types');
  }
}
