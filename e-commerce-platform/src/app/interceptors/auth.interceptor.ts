import { HttpInterceptorFn } from '@angular/common/http';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');
  
  if (token && req.url.includes('api/v1')) {
    const clonedReq = req.clone({
      headers: req.headers
        .set('Authorization', `Bearer ${token}`)
        .set('Content-Type', 'application/json')
    });
    console.log('Auth header added:', clonedReq.headers.get('Authorization'));
    return next(clonedReq);
  }
  
  return next(req);
};