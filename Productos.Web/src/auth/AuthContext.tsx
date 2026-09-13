import { createContext, useContext, useState, type ReactNode } from "react";

interface Sesion {
  token: string;
  email: string;
  rol: string;
}

interface AuthContextValue {
  sesion: Sesion | null;
  guardarSesion: (sesion: Sesion) => void;
  cerrarSesion: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

const STORAGE_KEY = "productos.sesion";

function leerSesionGuardada(): Sesion | null {
  const raw = localStorage.getItem(STORAGE_KEY);
  if (!raw) return null;
  try {
    return JSON.parse(raw) as Sesion;
  } catch {
    return null;
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [sesion, setSesion] = useState<Sesion | null>(leerSesionGuardada);

  function guardarSesion(nuevaSesion: Sesion) {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(nuevaSesion));
    setSesion(nuevaSesion);
  }

  function cerrarSesion() {
    localStorage.removeItem(STORAGE_KEY);
    setSesion(null);
  }

  return (
    <AuthContext.Provider value={{ sesion, guardarSesion, cerrarSesion }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth debe usarse dentro de AuthProvider");
  }
  return context;
}
