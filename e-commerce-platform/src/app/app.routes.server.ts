import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: 'products/:id',
    renderMode: RenderMode.Prerender,
    getPrerenderParams: async () => {
      // Example: Fetch a single product ID as a string
      const productId = await fetchProductId();
      return [{ id: productId }];
    }
  },
  {
    path: 'products/update/:id',
    renderMode: RenderMode.Prerender,
    getPrerenderParams: async () => {
      // Example: Fetch a single product ID as a string
      const productId = await fetchProductId();
      return [{ id: productId }];
    }
  },
  {
    path: '**',
    renderMode: RenderMode.Prerender
  }
];

// Example function to fetch a single product ID as a string
async function fetchProductId(): Promise<string> {
  // Replace this with actual logic to fetch a product ID
  return '019387a8-3633-7908-b4ac-ae6b9c0a0716';
}