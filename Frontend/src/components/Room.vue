<template>
  Room id:
  {{room.id}}
<draggable-panel
    class="draggable-panel"
    :canvas-style="{ ['margin-top']: '-9%' }"
    :chart-style="{['border-radius']: '25px'}"
    :width="1920"
    :height="1080"
    :padding="32"
    :data="bObjectsLoaded"
    :chart-min-width="0"
    :chart-min-height="0"
    :scale-min="0.5"
    :scale-max="10"
    :lock="true"
    :chartId="objectId"
    :scale="0.5"
    @char-click="selectChart"
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
</template>

<script setup lang="ts">
import DraggablePanel from '@/components/redactor/redactor';
import { ChartItem } from '@/types/draggable-panel';
import { Room } from '@/model/room';
import { useBookableObjectStore } from '@/stores/bookableObjectStore';

import { useRoomStore } from '@/stores/roomStore';
import { storeToRefs } from 'pinia';
import { Ref, ref, watch } from 'vue';

const props = defineProps<{
  facilityId: string,
}>();
const emit = defineEmits([
  'selectChart',
]);

const objectId = ref('');
const selectChart= (event: any, chart: ChartItem) =>{
      emit('selectChart' , chart);
    }
const img3 = '/src/components/redactor/svg/seats.svg'

const roomStore = useRoomStore();
let roomsLoaded = ref<Room[]>([]);
const room: Ref<Room> | undefined = ref<Room>({});
const { rooms } = storeToRefs(roomStore);

const bObjectStore = useBookableObjectStore();
let bObjectsLoaded = ref<ChartItem[]>([]);
const bObject: Ref<ChartItem> | undefined = ref<ChartItem>({});
const { bObjects } = storeToRefs(bObjectStore);

const loadRooms = async () => {
  await roomStore.load(props.facilityId);
  roomsLoaded.value = roomStore.rooms;
  return roomsLoaded;
};

const loadBObjects = async () => {
  await bObjectStore.load(room.value.id!);
  bObjectsLoaded.value = bObjectStore.bObjects;
  return bObjectsLoaded;
};

watch(()=>props.facilityId,async () => {
  await loadRooms();
  room.value = roomsLoaded.value[0];
});

watch(()=>room.value.id,async () => {
  await loadBObjects();
});



</script>

