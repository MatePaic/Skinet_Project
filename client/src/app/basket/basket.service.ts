import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }

  public getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id)
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          this._calculateTotals();
        })
      );
  }

  public addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this._mapProductItemToBasketItem(item, quantity);
    const basket = this._getCurrentBasketValue() ?? this._createBasket();
    basket.items = this._addOrUpdateItem(basket.items, itemToAdd, quantity);
    this._setBasket(basket);
  }

  public incrementItemQuantity(item: IBasketItem) {
    const basket = this._getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this._setBasket(basket);
  }

  public decrementItemQuantity(item: IBasketItem) {
    const basket = this._getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if(basket.items[foundItemIndex].quantity > 1) {
      basket.items[foundItemIndex].quantity--;
      this._setBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }
  }

  public removeItemFromBasket(item: IBasketItem) {
    const basket = this._getCurrentBasketValue();
    if (basket.items.some(x => x.id === item.id)) {
      basket.items = basket.items.filter(i => i.id !== item.id);
      if (basket.items.length > 0) {
        this._setBasket(basket);
      } else {
        this._deleteBasket(basket);
      }
    }
  }

  private _mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType
    };
  }

  private _getCurrentBasketValue() {
    return this.basketSource.value;
  }

  private _createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private _addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number) {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  private _setBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: IBasket) => {
      this.basketSource.next(response);
      this._calculateTotals();
    }, error => {
      console.log(error);
    });
  }

  private _calculateTotals() {
    const basket = this._getCurrentBasketValue();
    const shipping = 0;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;
    this.basketTotalSource.next({shipping, total, subtotal});
  }

  private _deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });
  }
}
