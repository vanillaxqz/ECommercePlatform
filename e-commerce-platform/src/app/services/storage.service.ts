import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  private memoryStorage: Map<string, string>;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.memoryStorage = new Map<string, string>();
  }

  private isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  getItem(key: string): string | null 
  {
    if (this.isBrowser()) {
      // Use localStorage in the browser
      return localStorage.getItem(key);
    }
    // Fallback to memory storage on the server
    return this.memoryStorage.get(key) || null;
  }

  setItem(key: string, value: string): void {
    if (this.isBrowser()) {
      // Use localStorage in the browser
      localStorage.setItem(key, value);
    } else {
      // Fallback to memory storage on the server
      this.memoryStorage.set(key, value);
    }
  }

  removeItem(key: string): void {
    if (this.isBrowser()) {
      // Use localStorage in the browser
      localStorage.removeItem(key);
    } else {
      // Fallback to memory storage on the server
      this.memoryStorage.delete(key);
    }
  }
}
