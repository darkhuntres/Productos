import { useState, type FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api/auth";
import { useAuth } from "../auth/AuthContext";
import { ApiError } from "../api/client";

export function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [cargando, setCargando] = useState(false);
  const { guardarSesion } = useAuth();
  const navigate = useNavigate();

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError("");
    setCargando(true);

    try {
      const respuesta = await login(email, password);
      guardarSesion({ token: respuesta.token, email: respuesta.email, rol: respuesta.rol });
      navigate("/productos");
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("No se pudo conectar con el servidor.");
      }
    } finally {
      setCargando(false);
    }
  }

  return (
    <div style={{ maxWidth: 300, margin: "60px auto" }}>
      <h1>Iniciar sesión</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="email">Email</label>
          <br />
          <input
            id="email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="password">Contraseña</label>
          <br />
          <input
            id="password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <br />
        <button type="submit" disabled={cargando}>
          {cargando ? "Ingresando..." : "Ingresar"}
        </button>
      </form>
      {error && <p style={{ color: "red" }}>{error}</p>}
    </div>
  );
}
