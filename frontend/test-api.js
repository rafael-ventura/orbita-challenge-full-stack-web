const axios = require('axios');

async function testAPI() {
  try {
    console.log('Testando conexão com a API...');
    
    // Teste 1: Verificar se o servidor está rodando
    const response = await axios.get('http://localhost:5264/api/students', {
      timeout: 5000
    });
    
    console.log('✅ API está funcionando!');
    console.log('Status:', response.status);
    console.log('Dados:', response.data);
    
  } catch (error) {
    console.error('❌ Erro ao conectar com a API:');
    console.error('Status:', error.response?.status);
    console.error('Mensagem:', error.message);
    
    if (error.code === 'ECONNREFUSED') {
      console.error('💡 O backend não está rodando. Execute:');
      console.error('cd ../backend/StudentManagement.API');
      console.error('dotnet run');
    }
  }
}

testAPI(); 