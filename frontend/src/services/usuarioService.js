import api from "../api/axios.js";

export const obtenerUsuarios = async () => {
  const response = await api.get("/usuarios");
  return response.data;
};

export const obtenerUsuario = async (id) => {
  const response = await api.get(`/usuarios/${id}`);
  return response.data;
};

export const crearUsuario = async (usuario) => {
  const response = await api.post(
    "/usuarios",
    usuario
  );

  return response.data;
};

export const actualizarUsuario = async (
  id,
  usuario
) => {
  await api.put(
    `/usuarios/${id}`,
    usuario
  );
};

export const desactivarUsuario = async (
  id
) => {
  await api.delete(
    `/usuarios/${id}`
  );
};

export const obtenerRoles = async () => {
  const response =
    await api.get(
      "/roles"
    );

  return response.data;
};  