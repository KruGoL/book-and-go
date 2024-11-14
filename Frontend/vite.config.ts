import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import * as path from 'path'
import { quasar, transformAssetUrls } from '@quasar/vite-plugin'
import WindiCSS from 'vite-plugin-windicss'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
		vue({
      template: { transformAssetUrls }
    }),
    quasar({
      sassVariables: 'src/quasar-variables.sass'}),
    WindiCSS(),
	],
  
  resolve: {
    alias: { '@': path.resolve(__dirname, 'src') },
   }, 
})
