export interface OrderDetails {
    orderId: string;
    orderDate: string;
    status: number;
    paymentId: string;
    userId: string;
    total?: number;
  }
  
  export interface OrderRequest {
    userId: string;
    orderDate: string;
    status: number;
    paymentId: string;
  }