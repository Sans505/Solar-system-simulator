const express = require('express');
const { Pool } = require('pg');
const cors = require('cors');

const app = express();
app.use(express.json());
app.use(cors());

// PEGA AQUÍ LA CADENA QUE COPIASTE DE NEON (Botón "Connect")
const connectionString = 'postgresql://usuario:password@host/neondb?sslmode=require';

const pool = new Pool({
  connectionString: connectionString,
  ssl: { rejectUnauthorized: false }
});

// Endpoint de LOGIN (Basado en tus capturas de Neon)
app.post('/login', async (req, res) => {
  const { correo, password } = req.body;
  try {
    const result = await pool.query(
      'SELECT * FROM usuarios WHERE correo_electronico = $1 AND contrasena = $2',
      [correo, password]
    );

    if (result.rows.length > 0) {
      res.json({ 
        success: true, 
        message: "¡Bienvenido!", 
        user: result.rows[0].nombre_usuario 
      });
    } else {
      res.status(401).json({ success: false, message: "Correo o contraseña incorrectos" });
    }
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

app.listen(3000, () => console.log('API lista en http://localhost:3000'));