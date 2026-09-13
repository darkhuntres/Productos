import { useEffect, useState, type FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import { ApiError } from "../api/client";
import {
  actualizarProducto,
  crearProducto,
  eliminarProducto,
  listarProductos,
  listarTiposProducto,
} from "../api/productos";
import type { Producto, ProductoFormData, TipoProducto } from "../types";

const FORM_VACIO: ProductoFormData = {
  nombre: "",
  descripcion: "",
  precio: "",
  stock: "",
  tipoProductoId: "",
};

export function Productos() {
  const { sesion, cerrarSesion } = useAuth();
  const navigate = useNavigate();
  const esAdmin = sesion?.rol === "Admin";

  const [productos, setProductos] = useState<Producto[]>([]);
  const [tipos, setTipos] = useState<TipoProducto[]>([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState("");
  const [mensaje, setMensaje] = useState("");

  const [mostrarForm, setMostrarForm] = useState(false);
  const [editandoId, setEditandoId] = useState<number | null>(null);
  const [form, setForm] = useState<ProductoFormData>(FORM_VACIO);
  const [erroresForm, setErroresForm] = useState<string[]>([]);

  useEffect(() => {
    cargarDatos();
  }, []);

  async function cargarDatos() {
    setCargando(true);
    setError("");
    try {
      const [listaProductos, listaTipos] = await Promise.all([
        listarProductos(),
        listarTiposProducto(),
      ]);
      setProductos(listaProductos);
      setTipos(listaTipos);
    } catch (err) {
      manejarErrorSesion(err);
    } finally {
      setCargando(false);
    }
  }

  function manejarErrorSesion(err: unknown) {
    if (err instanceof ApiError && err.status === 401) {
      cerrarSesion();
      navigate("/login");
      return;
    }
    setError(err instanceof ApiError ? err.message : "Ocurrió un error inesperado.");
  }

  function handleLogout() {
    cerrarSesion();
    navigate("/login");
  }

  function abrirFormularioNuevo() {
    setForm(FORM_VACIO);
    setEditandoId(null);
    setErroresForm([]);
    setMostrarForm(true);
  }

  function abrirFormularioEditar(producto: Producto) {
    setForm({
      nombre: producto.nombre,
      descripcion: producto.descripcion ?? "",
      precio: String(producto.precio),
      stock: String(producto.stock),
      tipoProductoId: String(producto.tipoProductoId),
    });
    setEditandoId(producto.id);
    setErroresForm([]);
    setMostrarForm(true);
  }

  function cerrarFormulario() {
    setMostrarForm(false);
    setEditandoId(null);
  }

  function validarFormulario(): string[] {
    const errores: string[] = [];

    if (!form.nombre.trim()) {
      errores.push("El nombre es obligatorio.");
    }
    if (!form.tipoProductoId) {
      errores.push("Debe seleccionar un tipo de producto.");
    }

    const precio = Number(form.precio);
    if (form.precio.trim() === "" || Number.isNaN(precio) || precio <= 0) {
      errores.push("El precio debe ser un número mayor a 0.");
    }

    const stock = Number(form.stock);
    if (form.stock.trim() === "" || Number.isNaN(stock) || stock < 0 || !Number.isInteger(stock)) {
      errores.push("El stock debe ser un número entero mayor o igual a 0.");
    }

    return errores;
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    const errores = validarFormulario();
    setErroresForm(errores);
    if (errores.length > 0) {
      return;
    }

    const payload = {
      nombre: form.nombre.trim(),
      descripcion: form.descripcion.trim() || null,
      precio: Number(form.precio),
      stock: Number(form.stock),
      tipoProductoId: Number(form.tipoProductoId),
    };

    try {
      if (editandoId) {
        await actualizarProducto(editandoId, payload);
        setMensaje("Producto actualizado.");
      } else {
        await crearProducto(payload);
        setMensaje("Producto creado.");
      }
      cerrarFormulario();
      await cargarDatos();
    } catch (err) {
      if (err instanceof ApiError) {
        setErroresForm([err.message]);
      } else {
        setErroresForm(["No se pudo guardar el producto."]);
      }
    }
  }

  async function handleEliminar(producto: Producto) {
    const confirmado = window.confirm(`¿Eliminar el producto "${producto.nombre}"?`);
    if (!confirmado) return;

    try {
      await eliminarProducto(producto.id);
      setMensaje("Producto eliminado.");
      await cargarDatos();
    } catch (err) {
      manejarErrorSesion(err);
    }
  }

  return (
    <div style={{ maxWidth: 800, margin: "40px auto" }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <h1>Productos</h1>
        <div>
          <span>
            {sesion?.email} ({sesion?.rol})
          </span>
          {" — "}
          <button type="button" onClick={handleLogout}>
            Cerrar sesión
          </button>
        </div>
      </div>

      {error && <p style={{ color: "red" }}>{error}</p>}
      {mensaje && <p style={{ color: "green" }}>{mensaje}</p>}

      {esAdmin && !mostrarForm && (
        <button type="button" onClick={abrirFormularioNuevo}>
          Nuevo producto
        </button>
      )}

      {mostrarForm && (
        <form onSubmit={handleSubmit} style={{ border: "1px solid #999", padding: 10, margin: "10px 0" }}>
          <h3>{editandoId ? "Editar producto" : "Nuevo producto"}</h3>

          <div>
            <label htmlFor="nombre">Nombre</label>
            <br />
            <input
              id="nombre"
              value={form.nombre}
              onChange={(e) => setForm({ ...form, nombre: e.target.value })}
            />
          </div>

          <div>
            <label htmlFor="descripcion">Descripción</label>
            <br />
            <input
              id="descripcion"
              value={form.descripcion}
              onChange={(e) => setForm({ ...form, descripcion: e.target.value })}
            />
          </div>

          <div>
            <label htmlFor="precio">Precio</label>
            <br />
            <input
              id="precio"
              value={form.precio}
              onChange={(e) => setForm({ ...form, precio: e.target.value })}
            />
          </div>

          <div>
            <label htmlFor="stock">Stock</label>
            <br />
            <input
              id="stock"
              value={form.stock}
              onChange={(e) => setForm({ ...form, stock: e.target.value })}
            />
          </div>

          <div>
            <label htmlFor="tipoProductoId">Tipo de producto</label>
            <br />
            <select
              id="tipoProductoId"
              value={form.tipoProductoId}
              onChange={(e) => setForm({ ...form, tipoProductoId: e.target.value })}
            >
              <option value="">-- Seleccione --</option>
              {tipos.map((tipo) => (
                <option key={tipo.id} value={tipo.id}>
                  {tipo.nombre}
                </option>
              ))}
            </select>
          </div>

          {erroresForm.length > 0 && (
            <ul style={{ color: "red" }}>
              {erroresForm.map((mensajeError) => (
                <li key={mensajeError}>{mensajeError}</li>
              ))}
            </ul>
          )}

          <br />
          <button type="submit">Guardar</button>{" "}
          <button type="button" onClick={cerrarFormulario}>
            Cancelar
          </button>
        </form>
      )}

      {cargando ? (
        <p>Cargando...</p>
      ) : (
        <table border={1} cellPadding={6} style={{ borderCollapse: "collapse", width: "100%", marginTop: 10 }}>
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Descripción</th>
              <th>Precio</th>
              <th>Stock</th>
              <th>Tipo</th>
              {esAdmin && <th>Acciones</th>}
            </tr>
          </thead>
          <tbody>
            {productos.length === 0 && (
              <tr>
                <td colSpan={esAdmin ? 6 : 5}>No hay productos registrados.</td>
              </tr>
            )}
            {productos.map((producto) => (
              <tr key={producto.id}>
                <td>{producto.nombre}</td>
                <td>{producto.descripcion}</td>
                <td>{producto.precio.toFixed(2)}</td>
                <td>{producto.stock}</td>
                <td>{producto.tipoProductoNombre}</td>
                {esAdmin && (
                  <td>
                    <button type="button" onClick={() => abrirFormularioEditar(producto)}>
                      Editar
                    </button>{" "}
                    <button type="button" onClick={() => handleEliminar(producto)}>
                      Eliminar
                    </button>
                  </td>
                )}
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
