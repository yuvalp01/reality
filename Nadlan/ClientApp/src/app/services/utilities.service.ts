import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UtilitiesService {

  constructor() { }

   fixUtcDate(dateIn) {
    ///fix UTC issue:
    let date = new Date(dateIn);
    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + 12);
    ///
  }
}