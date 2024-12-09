import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  private memoryStorage: Map<string, string>;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.memoryStorage = new Map<string, string>();
  }

  private isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  getItem(key: string): string | null {
    console.log('Accessing StorageService:', key, this.isBrowser() ? 'Browser' : 'Server');
    return this.memoryStorage.get(key) || null;
  }

  setItem(key: string, value: string): void {
    this.memoryStorage.set(key, value);
  }

  removeItem(key: string): void {
    this.memoryStorage.delete(key);
  }
}