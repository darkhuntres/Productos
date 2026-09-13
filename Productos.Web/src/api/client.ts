const API_URL = import.meta.env.VITE_API_URL;
const STORAGE_KEY = "productos.sesion";

function obtenerToken(): string | null {
  const raw = localStorage.getItem(STORAGE_KEY);
  if (!raw) return null;
  try {
    return (JSON.parse(raw) as { token: string }).token;
  } catch {
    return null;
  }
}

export class ApiError extends Error {
  status: number;

  constructor(status: number, message: string) {
    super(message);
    this.status = status;
  }
}

export async function apiFetch<T>(path: string, options: RequestInit = {}): Promise<T> {
  const token = obtenerToken();
  const headers = new Headers(options.headers);
  headers.set("Content-Type", "application/json");
  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }

  const response = await fetch(`${API_URL}${path}`, { ...options, headers });

  if (response.status === 204) {
    return undefined as T;
  }

  const texto = await response.text();
  const data = texto ? JSON.parse(texto) : null;

  if (!response.ok) {
    const mensaje = data?.mensaje ?? data?.title ?? "Ocurrió un error al comunicarse con el servidor.";
    throw new ApiError(response.status, mensaje);
  }

  return data as T;
}
