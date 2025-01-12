export interface OrderDetails {
    orderId: string;
    orderDate: string;
    status: number;
    paymentId: string;
    userId: string;
  }
  
  export interface OrderRequest {
    userId: string;
    orderDate: string;
    status: number;
    paymentId: string;
  }