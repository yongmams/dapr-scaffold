import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  root: 'src/',
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src/main'),
      '@login': path.resolve(__dirname, './src/login')
    }
  },
  build: {
    // ...
    // 指定输出目录
    outDir: path.resolve(__dirname, 'dist'),
    // rollup 配置打包项
    rollupOptions: {
      // ...
      input: {
        main: path.resolve(__dirname, './src/index.html'),
        login: path.resolve(__dirname, './src/login.html')
      }
    }
  },
  plugins: [react()],
  server: {
    proxy: {
      '/admin/api': {
        target: 'http://localhost:32771/',
        secure: false,
        changeOrigin: false
      },
      '/sso': {
        target: 'http://localhost/',
        secure: false,
        changeOrigin: false
      },
      '/zipkin': {
        target: 'http://localhost/',
        secure: false,
        changeOrigin: false
      },
      '/kb': {
        target: 'http://localhost/',
        secure: false,
        changeOrigin: false
      },
      '/redis': {
        target: 'http://localhost/',
        secure: false,
        changeOrigin: false
      },
      '/sqlpad': {
        target: 'http://localhost/',
        secure: false,
        changeOrigin: false
      }
    }
  }
})
