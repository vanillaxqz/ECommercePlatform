import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { StorageService } from '../services/storage.service'; // Import StorageService

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const storageService = inject(StorageService); // Inject StorageService
  const token = storageService.getItem('token'); // Use StorageService to get the token

  console.log('Original Request:', {
    url: req.url,
    headers: req.headers.keys(),
    hasToken: !!token
  });

  // Match any request to the API domain
  if (token && req.url.includes('ecommerceproiect.site')) {
    // Create new headers object with auth token
    const authReq = req.clone({
      setHeaders: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });

    console.log('Modified Request:', {
      url: authReq.url,
      headers: authReq.headers.keys(),
      authHeader: authReq.headers.get('Authorization')
    });

    return next(authReq);
  }

  return next(req);
};