import { HttpInterceptorFn, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { StorageService } from '../services/storage.service'; // Import StorageService

export const AuthInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const storageService = inject(StorageService); // Inject StorageService
  var token = storageService.getItem('token');

  console.log(`Original Request ${token}:`, {
    url: req.url,
    headers: req.headers.keys(),
    hasToken: !!token
  });

  // Match any request to the API domain
  if (token && req.url.includes('v1')) {
    // Create new headers object with auth token
    const authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`),
    });
    console.log('Modified Request:', {
      url: authReq.url,
      headers: authReq.headers.keys(),
      authHeader: authReq.headers.get('Authorization')
    });
    token = storageService.getItem('token');
    return next(authReq);
  }

  return next(req);
};