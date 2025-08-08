import { computed, Injectable, signal, Signal, WritableSignal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PresentationService {

  public currencySignal: Signal<string> = computed(() => this.currencyInternalSignal());
  private currencyInternalSignal: WritableSignal<string> = signal('local');

  constructor() { }

  setCurrency(value: string)
  {
    if (value != 'local' && value != 'USD')
    {
      throw 'Invalid value for currency: ' + value;
    }

    this.currencyInternalSignal.set(value);
  }
}
