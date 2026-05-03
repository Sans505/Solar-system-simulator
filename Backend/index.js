const express = require('express');
const { Pool } = require('pg');
const cors = require('cors');

const app = express();
app.use(express.json());
app.use(cors());

// --- CONFIGURACIÓN DE TU BASE DE DATOS NEON ---
const connectionString = 'postgresql://neondb_owner:npg_8OxX9jqlvmnT@ep-shy-thunder-ama8zfq2-pooler.c-5.us-east-1.aws.neon.tech/neondb?sslmode=require&channel_binding=require';

const pool = new Pool({
  connectionString: connectionString,
  ssl: { rejectUnauthorized: false }
});

// --- RUTA DE LOGIN ---
app.post('/login', async (req, res) => {
  const { correo, password } = req.body;
  
  console.log("----------------------------");
  console.log("Petición desde Unity recibida:");
  console.log("Correo:", correo);
  console.log("Pass:", password);

  try {
    // IMPORTANTE: Los nombres de columnas coinciden exactamente con tu captura de Neon
    const result = await pool.query(
      'SELECT * FROM usuarios WHERE correo_electronico = $1 AND contrasena = $2',
      [correo, password]
    );

    console.log("Usuarios encontrados en la BD:", result.rows.length);

    if (result.rows.length > 0) {
      console.log("¡ÉXITO! Usuario autenticado:", result.rows[0].nombre_usuario);
      res.json({ 
        success: true, 
        user: result.rows[0].nombre_usuario 
      });
    } else {
      console.log("FALLO: El correo o la contraseña no coinciden en Neon.");
      res.status(401).json({ success: false, message: "Credenciales incorrectas" });
    }
  } catch (err) {
    console.error("ERROR CRÍTICO CONEXIÓN NEON:", err.message);
    res.status(500).json({ error: err.message });
  }
});

// --- INICIAR SERVIDOR ---
const PORT = 3000;
app.listen(PORT, () => {
  console.log(`>>> API LISTA Y CONECTADA A NEON <<<`);
  console.log(`Escuchando en: http://localhost:${PORT}`);
});