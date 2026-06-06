import api from "../api/axios";

export const obtenerEnvios =
  async () => {
    const response =
      await api.get("/envios");

    return response.data;
  };

export const obtenerEnvio =
  async (id) => {
    const response =
      await api.get(`/envios/${id}`);

    return response.data;
  };

export const crearEnvio =
  async (envio) => {
    const response =
      await api.post(
        "/envios",
        envio
      );

    return response.data;
  };

export const actualizarEnvio =
  async (
    id,
    envio
  ) => {
    const response =
      await api.put(
        `/envios/${id}`,
        envio
      );

    return response.data;
  };