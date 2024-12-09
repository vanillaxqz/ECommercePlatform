import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';

import { appRoutes } from './app.routes';
import { AuthInterceptor } from './interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ 
      eventCoalescing: true 
    }),
    provideRouter(appRoutes),
    provideClientHydration(
      withEventReplay()
    ),
    provideHttpClient(
      withFetch(),
      withInterceptors([AuthInterceptor]),
      withInterceptorsFromDi()
    )
  ]
};