<template>
  <div class="container mt-4">
    <h2 class="text-center mb-4">Comparar Imagens</h2>
    
    <div class="d-flex justify-content-center">
      <!-- Upload Foto 1 -->
      <div class="form-group mx-3 text-center">
        <label>Imagem 1</label>
        <div ref="dropzone1" class="dropzone"></div>
        <div v-if="imagemBase64_1" class="preview mt-3">
          <img :src="'data:image/jpeg;base64,' + imagemBase64_1" class="img-fluid img-preview" />
        </div>
      </div>

      <!-- Upload Foto 2 -->
      <div class="form-group mx-3 text-center">
        <label>Imagem 2</label>
        <div ref="dropzone2" class="dropzone"></div>
        <div v-if="imagemBase64_2" class="preview mt-3">
          <img :src="'data:image/jpeg;base64,' + imagemBase64_2" class="img-fluid img-preview" />
        </div>
      </div>
    </div>

    <div class="text-center mt-4">
      <button 
        type="button" 
        class="btn btn-primary" 
        @click="compararImagens"
        :disabled="!imagemBase64_1 || !imagemBase64_2 || loading"
      >
        <span v-if="loading">
          <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
          Processando...
        </span>
        <span v-else>
          <i class="fa fa-search"></i> Comparar
        </span>
      </button>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";
import Dropzone from "dropzone";
import "dropzone/dist/dropzone.css";
import axios from "axios";

export default {
  data() {
    return {
      imagemBase64_1: "",
      imagemBase64_2: "",
      loading: false,
      dropzone1: null,
      dropzone2: null
    };
  },
  mounted() {
    this.initDropzones();
  },
  methods: {
    initDropzones() {
      this.dropzone1 = new Dropzone(this.$refs.dropzone1, this.getDropzoneConfig(1));
      this.dropzone2 = new Dropzone(this.$refs.dropzone2, this.getDropzoneConfig(2));
    },
    getDropzoneConfig(index) {
      const self = this;
      return {
        url: "/",
        autoProcessQueue: false,
        acceptedFiles: "image/*",
        maxFiles: 1,
        addRemoveLinks: true,
        dictDefaultMessage: "Arraste uma imagem aqui ou clique para selecionar",
        init: function () {
          this.on("addedfile", (file) => {
            if (file.size > 5 * 1024 * 1024) { // 5MB max
              Swal.fire("Erro", "A imagem não pode ser maior que 5MB", "error");
              this.removeFile(file);
              return;
            }
            
            self.converterImagemBase64(file).then((base64) => {
              if (index === 1) self.imagemBase64_1 = base64.split(",")[1];
              else self.imagemBase64_2 = base64.split(",")[1];
            });
          });
          
          this.on("removedfile", () => {
            if (index === 1) self.imagemBase64_1 = "";
            else self.imagemBase64_2 = "";
          });
        }
      };
    },
    async converterImagemBase64(file) {
      return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = (event) => resolve(event.target.result);
        reader.onerror = (error) => reject(error);
        reader.readAsDataURL(file);
      });
    },
    async compararImagens() {
      if (!this.imagemBase64_1 || !this.imagemBase64_2) {
        Swal.fire("Erro", "Ambas as imagens são obrigatórias!", "error");
        return;
      }

      this.loading = true;
      
      try {
        const response = await axios.post("https://localhost:7269/api/v1/usuarios/comparar-imagens", {
          base64Imagem1: this.imagemBase64_1,
          base64Imagem2: this.imagemBase64_2
        });

        const similaridade = response.data.similaridade;
        
        if (similaridade > 70) {
          Swal.fire({
            title: "Similaridade Detectada!",
            html: `As imagens têm <strong>${similaridade.toFixed(2)}%</strong> de similaridade`,
            icon: "success",
            confirmButtonText: "OK"
          });
        } else {
          Swal.fire({
            title: "Baixa Similaridade",
            html: `Similaridade detectada: <strong>${similaridade.toFixed(2)}%</strong>`,
            icon: "warning",
            confirmButtonText: "OK"
          });
        }
      } catch (error) {
        console.error("Erro ao comparar imagens:", error);
        Swal.fire({
          title: "Erro",
          text: error.response?.data?.message || "Ocorreu um erro ao comparar as imagens",
          icon: "error",
          confirmButtonText: "OK"
        });
      } finally {
        this.loading = false;
      }
    },
    resetDropzones() {
      this.dropzone1.removeAllFiles(true);
      this.dropzone2.removeAllFiles(true);
      this.imagemBase64_1 = "";
      this.imagemBase64_2 = "";
    }
  }
};
</script>

<style>
.img-preview {
  max-width: 100%;
  max-height: 300px;
  border-radius: 10px;
  border: 3px solid #ddd;
  object-fit: contain;
}

.dropzone {
  border: 2px dashed #007bff;
  border-radius: 10px;
  padding: 20px;
  text-align: center;
  background-color: #f8f9fa;
  width: 300px;
  min-height: 150px;
  transition: all 0.3s ease;
}

.dropzone:hover {
  background-color: #e9f5ff;
  border-color: #0056b3;
}

.dropzone .dz-message {
  margin: 2em 0;
  color: #6c757d;
}

.btn-primary {
  padding: 10px 25px;
  font-weight: 500;
}

@media (max-width: 768px) {
  .d-flex {
    flex-direction: column;
    align-items: center;
  }
  
  .form-group {
    margin-bottom: 20px;
  }
  
  .dropzone {
    width: 100%;
  }
}
</style>