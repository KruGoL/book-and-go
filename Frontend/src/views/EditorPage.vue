<template>
  <EditorMain :facility-id="facilityId"/>
</template>

<script setup lang="ts">
import EditorMain from '@/components/redactor/RedactorMain.vue';

import { Facility } from '@/model/facility';
import { useFacilitiesStore } from '@/stores/facilityStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, Ref } from 'vue';

const facilitiesStore = useFacilitiesStore();
let facilities = ref<Facility[]>([]);
const facility: Ref<Facility> | undefined = ref<Facility>({});
const { Facilities } = storeToRefs(facilitiesStore);

const facilityId: Ref<string> = ref<string>('');

onMounted(async () => {
  await facilitiesStore.loadAcc();
  facilities = Facilities;
  facilityId.value = facilities.value[0].id!;
  facility.value = facilitiesStore.getAccFacilityById(facilityId.value)!;
});
</script>
