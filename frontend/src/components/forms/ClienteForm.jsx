import { useState } from "react";

export default function ClienteForm({
initialData,
onSubmit,
}) {

const [form,
setForm] = useState(
initialData || {
identificacion: "",
nombre: "",
direccion: "",
telefono: "",
}
);

const handleChange = e => {

setForm({
  ...form,
  [e.target.name]:
    e.target.value,
});

};

const handleSubmit = e => {

e.preventDefault();

onSubmit(form);

};

return ( <form
   onSubmit={handleSubmit}
 > <input
     name="identificacion"
     placeholder="Identificación"
     value={form.identificacion}
     onChange={handleChange}
   />

  <br />

  <input
    name="nombre"
    placeholder="Nombre"
    value={form.nombre}
    onChange={handleChange}
  />

  <br />

  <input
    name="direccion"
    placeholder="Dirección"
    value={form.direccion}
    onChange={handleChange}
  />

  <br />

  <input
    name="telefono"
    placeholder="Teléfono"
    value={form.telefono}
    onChange={handleChange}
  />

  <br />

  <button type="submit">
    Guardar
  </button>
</form>

);
}
