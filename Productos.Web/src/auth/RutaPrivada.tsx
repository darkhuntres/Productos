import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthContext";

export function RutaPrivada() {
  const { sesion } = useAuth();

  if (!sesion) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
}
