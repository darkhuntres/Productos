export interface LoginResponse {
  token: string;
  email: string;
  rol: string;
  expiraEn: string;
}

export interface TipoProducto {
  id: number;
  nombre: string;
}

export interface Producto {
  id: number;
  nombre: string;
  descripcion: string | null;
  precio: number;
  stock: number;
  tipoProductoId: number;
  tipoProductoNombre: string;
}

export interface ProductoFormData {
  nombre: string;
  descripcion: string;
  precio: string;
  stock: string;
  tipoProductoId: string;
}
