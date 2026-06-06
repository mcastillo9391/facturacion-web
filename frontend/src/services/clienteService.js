import api from "../api/axios";

export const obtenerClientes = async () => {
  const response = await api.get("/clientes");
  return response.data;
};

export const obtenerCliente = async (id) => {
  const response = await api.get(`/clientes/${id}`);
  return response.data;
};

export const crearCliente = async (cliente) => {
  const response = await api.post("/clientes", cliente);
  return response.data;
};

export const actualizarCliente = async (
  id,
  cliente
) => {
  await api.put(`/clientes/${id}`, cliente);
};

export const eliminarCliente = async (id) => {
  await api.delete(`/clientes/${id}`);
};