import { apiFetch } from "./client";
import type { Producto, TipoProducto } from "../types";

export interface ProductoPayload {
  nombre: string;
  descripcion: string | null;
  precio: number;
  stock: number;
  tipoProductoId: number;
}

export function listarProductos(): Promise<Producto[]> {
  return apiFetch<Producto[]>("/api/productos");
}

export function listarTiposProducto(): Promise<TipoProducto[]> {
  return apiFetch<TipoProducto[]>("/api/tiposproducto");
}

export function crearProducto(payload: ProductoPayload): Promise<Producto> {
  return apiFetch<Producto>("/api/productos", {
    method: "POST",
    body: JSON.stringify(payload),
  });
}

export function actualizarProducto(id: number, payload: ProductoPayload): Promise<Producto> {
  return apiFetch<Producto>(`/api/productos/${id}`, {
    method: "PUT",
    body: JSON.stringify(payload),
  });
}

export function eliminarProducto(id: number): Promise<void> {
  return apiFetch<void>(`/api/productos/${id}`, { method: "DELETE" });
}
