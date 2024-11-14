import { Chart } from '@/model/Chart';
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useChartsStore = defineStore('chartsStore', () => {
  const charts = ref<Chart[]>([
    { id: '111', width: 168, height: 340, x: 390, y: 195 , img: "src/components/redactor/svg/table1.svg", seats: 4},
    { id: '222', width: 168, height: 340, x: 875, y: 195 ,img: "src/components/redactor/svg/table1.svg", seats: 4},
    { id: '333', width: 168, height: 340, x: 1360, y: 195 ,img: "src/components/redactor/svg/table1.svg", seats: 4},
    { id: '444', width: 172, height: 172, x: 380, y: 745 ,img: "src/components/redactor/svg/table1.svg", seats: 2},
    { id: '555', width: 172, height: 172, x: 878, y: 745 ,img: "src/components/redactor/svg/table1.svg", seats: 2},
    { id: '666', width: 172, height: 172, x: 1360, y: 745 ,img: "src/components/redactor/svg/table1.svg", seats: 2}
  ]);

  const addChart = (chart: Chart) => {
    charts.value.push(chart);
  };
  const deleteChart= (id : string)=>{
    const index = charts.value.findIndex((x) => x.id === id);
    if (index > -1) {
      charts.value.splice(index, 1);
    }
  };
  return { charts, addChart , deleteChart };
});
