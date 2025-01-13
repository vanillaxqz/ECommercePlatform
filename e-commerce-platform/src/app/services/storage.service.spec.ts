import { TestBed } from '@angular/core/testing';
import { StorageService } from './storage.service';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

describe('StorageService', () => {
  let service: StorageService;
  let platformId: Object;

  beforeEach(() => {
    platformId = 'browser'; // Mock platform ID as 'browser'

    TestBed.configureTestingModule({
      providers: [
        StorageService,
        { provide: PLATFORM_ID, useValue: platformId }
      ]
    });

    service = TestBed.inject(StorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get item from localStorage if running in browser', () => {
    spyOn(localStorage, 'getItem').and.returnValue('test-value');
    const result = service.getItem('test-key');
    expect(result).toBe('test-value');
    expect(localStorage.getItem).toHaveBeenCalledWith('test-key');
  });

  it('should set item in localStorage if running in browser', () => {
    spyOn(localStorage, 'setItem');
    service.setItem('test-key', 'test-value');
    expect(localStorage.setItem).toHaveBeenCalledWith('test-key', 'test-value');
  });

  it('should remove item from localStorage if running in browser', () => {
    spyOn(localStorage, 'removeItem');
    service.removeItem('test-key');
    expect(localStorage.removeItem).toHaveBeenCalledWith('test-key');
  });

  it('should get item from memoryStorage if not running in browser', () => {
    platformId = 'server'; // Mock platform ID as 'server'
    TestBed.overrideProvider(PLATFORM_ID, { useValue: platformId });
    service = TestBed.inject(StorageService);

    service.setItem('test-key', 'test-value');
    const result = service.getItem('test-key');
    expect(result).toBe('test-value');
  });

  it('should set item in memoryStorage if not running in browser', () => {
    platformId = 'server'; // Mock platform ID as 'server'
    TestBed.overrideProvider(PLATFORM_ID, { useValue: platformId });
    service = TestBed.inject(StorageService);

    service.setItem('test-key', 'test-value');
    const result = service.getItem('test-key');
    expect(result).toBe('test-value');
  });

  it('should remove item from memoryStorage if not running in browser', () => {
    platformId = 'server'; // Mock platform ID as 'server'
    TestBed.overrideProvider(PLATFORM_ID, { useValue: platformId });
    service = TestBed.inject(StorageService);

    service.setItem('test-key', 'test-value');
    service.removeItem('test-key');
    const result = service.getItem('test-key');
    expect(result).toBeNull();
  });
});