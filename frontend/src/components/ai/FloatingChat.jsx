import { enviarPregunta } from "../../services/chatService";
import "./FloatingChat.css";
import {
  useState,
  useEffect,
  useRef
} from "react";


export default function FloatingChat() {
  const [open, setOpen] = useState(false);

  const [messages, setMessages] = useState([
    {
      text: "Hola 👋 ¿En qué puedo ayudarte?",
      isUser: false,
    },
  ]);

  const [input, setInput] = useState("");

  const [loading, setLoading] =
    useState(false);

  const bottomRef = useRef(null);

  useEffect(() => {
    bottomRef.current?.scrollIntoView({
      behavior: "smooth",
    });
  }, [messages]);

  useEffect(() => {
    if (open) {
        setTimeout(() => {
        bottomRef.current?.scrollIntoView({
            behavior: "auto",
        });
        }, 50);
    }
    }, [open]);

  const clearChat = () => {
    setMessages([
        {
        text:
            "Hola 👋 ¿En qué puedo ayudarte?",
        isUser: false,
        },
    ]);
    };

  const sendMessage = async (
    text = input
    ) => {
    if (!text.trim()) return;

    setMessages((prev) => [
        ...prev,
        {
        text,
        isUser: true,
        },
    ]);

    setInput("");

    try {
        setLoading(true);

        const response =
        await enviarPregunta(text);

        setMessages((prev) => [
        ...prev,
        {
            text:
            response.message ??
            response,
            type:
            response.type ??
            "text",
            data:
            response.data ??
            null,
            isUser: false,
        },
        ]);
    }
    catch {
        setMessages((prev) => [
        ...prev,
        {
            text:
            "Error consultando el asistente.",
            isUser: false,
        },
        ]);
    }
    finally {
        setLoading(false);
    }
    };
  return (
    <>        
      <button
        className="chat-fab"
        onClick={() =>
          setOpen(!open)
        }
      >
        💬
      </button>

      {open && (
        <div className="chat-window">

          <div className="chat-header">
            Asistente IA
            <button
                className="chat-clear-btn"
                onClick={clearChat}
                title="Limpiar conversación"
            >
                ✕
            </button>
          </div>

          <div className="chat-body">

            {messages.map((msg, index) => (
                <div
                    key={index}
                    className={
                    msg.isUser
                        ? "user-message"
                        : "bot-message"
                    }
                >
                    {msg.type === "top_deudores" ? (
                    <>
                        <div
                        style={{
                            fontWeight: "600",
                            marginBottom: "8px",
                        }}
                        >
                        {msg.text}
                        </div>

                        {msg.data?.map(
                        (item, idx) => (
                            <div
                            key={idx}
                            style={{
                                display: "flex",
                                justifyContent:
                                "space-between",
                                marginBottom: "4px",
                            }}
                            >
                            <span>
                                {idx + 1}.{" "}
                                {item.cliente}
                            </span>

                            <strong>
                                $
                                {Number(
                                item.saldo
                                ).toLocaleString(
                                "es-CO"
                                )}
                            </strong>
                            </div>
                        )
                        )}
                        <div
                            style={{
                                display: "flex",
                                gap: "8px",
                                marginTop: "12px",
                                flexWrap: "wrap",
                            }}
                            >
                            <button
                                onClick={() =>
                                sendMessage(
                                    "¿Cuál es la cartera total?"
                                )
                                }
                            >
                                Cartera Total
                            </button>

                            <button
                                onClick={() =>
                                sendMessage(
                                    "¿Qué cliente debe más?"
                                )
                                }
                            >
                                Mayor Deudor
                            </button>

                            <button
                                onClick={() =>
                                window.location.href =
                                    "/cartera"
                                }
                            >
                                Ver Cartera
                            </button>
                            </div>
                    </>
                    ) : (
                    msg.text
                    )}
                </div>
                ))}

            {loading && (
              <div className="bot-message">
                Pensando...
              </div>
            )}

            <div ref={bottomRef} />            

          </div>

          <div className="chat-actions">
            <button
              onClick={() =>
                sendMessage(
                  "¿Cuál es la cartera total?"
                )
              }
            >
              Cartera
            </button>

            <button
              onClick={() =>
                sendMessage(
                  "¿Cuánto se ha pagado hoy?"
                )
              }
            >
              Pagos Hoy
            </button>

            <button
              onClick={() =>
                sendMessage(
                  "¿Qué cliente debe más?"
                )
              }
            >
              Mayor Deudor
            </button>
          </div>

          <div className="chat-footer">
            <input
              value={input}
              onChange={(e) =>
                setInput(
                  e.target.value
                )
              }
              placeholder="Pregunta algo..."
              onKeyDown={(e) => {
                if (
                  e.key === "Enter"
                ) {
                  sendMessage();
                }
              }}
            />

            <button
              onClick={() =>
                sendMessage()
              }
            >
              Enviar
            </button>
          </div>

        </div>
      )}
    </>
  );
}