import api from "../api/axios";

export const enviarPregunta = async (message) => {
  const response = await api.post("/chat", {
    message,
  });

  return response.data;
};