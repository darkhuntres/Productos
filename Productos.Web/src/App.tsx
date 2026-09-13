import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { AuthProvider } from "./auth/AuthContext";
import { RutaPrivada } from "./auth/RutaPrivada";
import { Login } from "./pages/Login";
import { Productos } from "./pages/Productos";

export function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route element={<RutaPrivada />}>
            <Route path="/productos" element={<Productos />} />
          </Route>
          <Route path="*" element={<Navigate to="/productos" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
