import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: 'products/:id',
    renderMode: RenderMode.Prerender,
    getPrerenderParams: async () => {
      // Example: Fetch all product IDs from a database or API
      const productIds = await fetchProductIds();
      return productIds.map(id => ({ id: id.toString() }));
    }
  },
  {
    path: 'products/update/:id',
    renderMode: RenderMode.Prerender,
    getPrerenderParams: async () => {
      // Example: Fetch all product IDs from a database or API
      const productIds = await fetchProductIds();
      return productIds.map(id => ({ id: id.toString() }));
    }
  },
  {
    path: '**',
    renderMode: RenderMode.Prerender
  }
];

// Example function to fetch product IDs
async function fetchProductIds(): Promise<number[]> {
  // Replace this with actual logic to fetch product IDs
  return [1, 2, 3, 4, 5];
}