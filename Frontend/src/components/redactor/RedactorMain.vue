<template>
  <div class="q-pa-md" style="max-width: 300px">
    <q-uploader
      :factory="factoryFn"
      @uploaded="updateImg"
      label="Upload room img"
      accept=".jpg, image/*"
      @rejected="onRejected"
      no-thumbnails
      auto-upload
      max-files="1"
    >
      <template v-slot:list="scope">
        <q-list separator>
          <q-item v-for="file in scope.files" :key="file.__key">
            <q-item-section>
              <q-item-label class="full-width ellipsis">
                {{ file.name }}
              </q-item-label>

              <q-item-label caption> Status: {{ file.__status }} </q-item-label>

            </q-item-section>

            <q-item-section v-if="file.__img" thumbnail class="gt-xs">
              <img :src="file.__img.src" />
            </q-item-section>

            <q-item-section top side>
              <q-btn
                class="gt-xs"
                size="12px"
                flat
                dense
                round
                icon="delete"
                @click="scope.removeFile(file)"
              />
            </q-item-section>
          </q-item>
        </q-list>
      </template>
    </q-uploader>
  </div>
  <div class="page">
    <transition name="toolbar">
      <div class="side" v-if="editStatus == true">
        <div draggable="true" @:dragstart="dragstartOutDP($event, img1)">
          <img :src="img1" alt="" />
        </div>
        <div draggable="true" @:dragstart="dragstartOutDP($event, img2)">
          <img :src="img2" alt="" />
        </div>
      </div>
    </transition>

    <draggable-panel
      class="draggable-panel"
      :chart-style="{ ['border-radius']: '25px' }"
      :width="1920"
      :height="1080"
      :padding="32"
      :data="bObjectsLoaded"
      :chart-min-width="0"
      :chart-min-height="0"
      :scale-min="0.5"
      :scale-max="10"
      :lock="!editStatus"
      :chartId="objectId"
      @canvas-scale="canvasScale"
      @canvas-drop="dropInCanvas"
      @char-click="selectChart"
      :scale="scale"
      :bg-img-src="room.img"
    >
      <template #chart="{ chart, index }">
        <div
          class="flex-1 absolute-center text-center text-light-50 font-medium text-17px"
          style="padding: 10px"
        >
          <img class="seats" :src="img3" />
          <text class="font-bold text-size-20px">{{ chart.seats }}</text>
        </div>
      </template>
    </draggable-panel>

    <div class="info-panel q-pa-md">
      <div class="text-light-100 mt-2">
        <div>
          <div class="mb-2">
            <text class="text-light-100 font-semibold text-20px">Table: </text>
            <text class="font-semibold text-20px text-light-blue-800">{{
              selBObject.id
            }}</text>
          </div>
          <q-separator class="bg-light-100" />
          <div class="mt-5 text-15px">
            <q-input
              dark
              :readonly="!editStatus"
              type="number"
              filled
              v-model.number="selBObject.x"
            >
              <template v-slot:before>
                <text class="text-light-100 text-20px">X :</text>
              </template>
            </q-input>
          </div>
          <div class="mt-5 text-15px">
            <q-input
              dark
              :readonly="!editStatus"
              type="number"
              filled
              v-model.number="selBObject.y"
            >
              <template v-slot:before>
                <text class="text-light-100 text-20px">Y :</text>
              </template>
            </q-input>
          </div>

          <div class="mt-5 text-15px">
            <q-input
              dark
              :readonly="!editStatus"
              type="number"
              filled
              v-model.number="selBObject.seats"
            >
              <template v-slot:before>
                <img :src="img3" style="width: 20px; height: 20px" />
              </template>
            </q-input>
          </div>
          <q-btn class="save-edit-btn" @click.prevent="saveOrEdit()">{{
            editStatusBtn
          }}</q-btn>
          <q-btn
            v-if="editStatus"
            class="delete-btn"
            icon="delete"
            color="red"
            @click.prevent="deleteChart()"
          />
        </div>
      </div>
    </div>
  </div>
  {{ facilityId }}
</template>

<script lang="ts">
import DraggablePanel from '@/components/redactor/redactor';
import { QSeparator, QInput, QBtn, useQuasar, QUploader } from 'quasar';
import { storeToRefs } from 'pinia';
import { defineComponent, ref, reactive, Ref } from 'vue';
import { v4 as uuidv4 } from 'uuid';

import { useRoomStore } from '@/stores/roomStore';

import { ChartItem } from '@/types/draggable-panel';
import { Room } from '@/model/room';
import { useBookableObjectStore } from '@/stores/bookableObjectStore';

export default defineComponent({
  name: 'App',

  components: {
    DraggablePanel,
  },
  data() {
    return {
      res: null,
      renderComponent: true,
      img1: '/src/components/redactor/svg/table1.svg',
      img2: '/src/components/redactor/svg/table2.svg',
      img3: '/src/components/redactor/svg/seats.svg',
    };
  },
  props: {
    facilityId: {
      required: true,
      type: String,
    },
  },
  setup(props) {
    const $q = useQuasar();

    const roomStore = useRoomStore();
    let roomsLoaded = ref<Room[]>([]);
    const room: Ref<Room> | undefined = ref<Room>({});
    const { rooms } = storeToRefs(roomStore);

    const bObjectStore = useBookableObjectStore();
    let bObjectsLoaded = ref<ChartItem[]>([]);
    const bObject: Ref<ChartItem> | undefined = ref<ChartItem>({});
    const selBObject: Ref<ChartItem> | undefined = ref<ChartItem>({});
    const { bObjects } = storeToRefs(bObjectStore);
    const objectId = ref('');

    const loadRooms = async () => {
      await roomStore.load(props.facilityId);
      roomsLoaded.value = roomStore.rooms;
      room.value = roomsLoaded.value[0]
      return roomsLoaded;
    };

    const loadBObjects = async () => {
      await bObjectStore.load(room.value.id!);
      bObjectsLoaded.value = bObjectStore.bObjects;
      return bObjectsLoaded;
    };

    const offset = reactive({ x: 0, y: 0 });
    const chartId = ref('');
    const img = ref('');
    const editStatus = ref(false);
    const editStatusBtn = ref('Edit');
    const scale = ref(1);
    const tempScale = ref(1);
    const file = ref();

    return {
      loadRooms,
      loadBObjects,
      roomsLoaded,
      room,
      bObjectStore,
      bObjectsLoaded,
      objectId,
      bObject,
      selBObject,
      offset,
      chartId,
      editStatus,
      editStatusBtn,
      img,
      scale,
      tempScale,
      file,
      onRejected(rejectedEntries: any) {
        // Notify plugin needs to be installed
        // https://quasar.dev/quasar-plugins/notify#Installation
        $q.notify({
          type: 'negative',
          message: 'file did not pass validation constraints',
        });
      },
    };
  },
  methods: {
    dragstartOutDP(event: DragEvent, img: string) {
      this.offset = {
        x: event.offsetX,
        y: event.offsetY,
      };
      this.img = img;
    },
    async dropInCanvas(event: any, position: any) {
      this.bObject.id = uuidv4();
      this.bObject.height = 100;
      this.bObject.width = 100;
      this.bObject.x = position.x - this.offset.x;
      this.bObject.y = position.y - this.offset.y;
      this.bObject.roomId = this.room.id;
      await this.bObjectStore.addBObject(this.bObject);
      await this.loadBObjects();
      this.selBObject = this.bObject;
    },
    canvasScale(scale: any) {
      this.scale = scale;
    },
    selectChart(event: any, bObject: ChartItem) {
      this.selBObject = bObject;
    },
    async saveOrEdit() {
      this.editStatus = !this.editStatus;
      if (this.editStatusBtn == 'Save') {
        this.editStatusBtn = 'Edit';
        this.scale = this.tempScale;
        await this.bObjectStore.updateBObject(this.selBObject);
      } else {
        this.editStatusBtn = 'Save';
        this.tempScale = this.scale;
      }
    },
    reset() {},
    async deleteChart() {
      await this.bObjectStore.deleteBObject(this.selBObject);
      await this.loadBObjects();
    },
    factoryFn(file : File) {
      return new Promise((resolve, reject) => {
        // Retrieve JWT token from your store.
        const uploadUrl =
          'https://localhost:7022/api/Upload/Room/' + this.room.id;
        const token = localStorage.getItem('token');
        resolve({
          url: uploadUrl,
          method: 'POST',
          headers: [{ name: 'Authorization', value: `Bearer ${token}` }],
        });
      });
    },
    async updateImg(){
      await this.loadRooms();
    }
  },

  watch: {
    async facilityId() {
      await this.loadRooms();
      //this.room = this.roomsLoaded[0];
    },
    async room() {
      await this.loadBObjects();
    },
  },
});
</script>

<style lang="scss">
body {
  margin: 0;
  font-family: 'Helvetica Neue', Helvetica, 'PingFang SC', 'Hiragino Sans GB',
    Arial, sans-serif;
}
</style>

<style lang="scss" scoped>
.page {
  height: 100vh;
  display: flex;
  flex-direction: row;
  background: #dddddd;
  flex-wrap: nowrap;
}
.info-panel {
  min-width: 260px;
  height: 100%;
  border-left: 1px solid #dddddd;
  background: #129bff;
  padding: 16px;
  box-sizing: border-box;
}
.save-edit-btn {
  height: 30px;
  width: 100px;
  background: #ffffff;
  color: #129bff;
  margin-left: 15%;
  margin-top: 10%;
  border-radius: 5px;
  bottom: 30%;
  position: relative;
}
.delete-btn {
  height: 30px;
  width: 50px;
  background: #ffffff;
  color: #cc0000;
  margin-left: 5%;
  margin-top: 10%;
  border-radius: 5px;
  bottom: 30%;
  position: relative;
}
.input {
  background: #176096;
  color: #ffffff;
}
.side {
  width: 260px;
  height: 100%;
  border-left: 1px solid #dddddd;
  background: #ffffff;
  padding: 16px;
  box-sizing: border-box;
}

.box {
  height: 100px;
  background: #66bfff;
  border-radius: 4px;
  padding: 8px;
  border: 1px solid #a8dbff;
  display: flex;
  justify-content: center;
  align-items: center;
  box-shadow: 0 5px 20px rgba(#333333, 0.1);
}
.box:hover {
  filter: brightness(0.9);
}
.circle {
  height: 100px;
  background: #66bfff;
  border-radius: 100%;
  padding: 8px;
  border: 1px solid #a8dbff;
  display: flex;
  justify-content: center;
  align-items: center;
  box-shadow: 0 5px 20px rgba(#333333, 0.1);
  margin-top: 10px;
}
.circle:hover {
  filter: brightness(0.9);
}
.txt {
  font-weight: 700;
  color: #0d3654;
}
.toolbar-enter-from {
  transform: translateX(-250px);
}
.toolbar-enter-to {
  transform: translateX(0);
}
.toolbar-enter-active {
  transition: all 0.6s ease;
}
.toolbar-leave-from {
  transform: translateX(0);
}
.toolbar-leave-to {
  transform: translateX(-250px);
}
.toolbar-leave-active {
  transition: all 0.6s ease;
}
.seats {
  width: 100%;
  height: auto;
  color: $left-menu-btn;
  opacity: 0.85;
}
</style>
